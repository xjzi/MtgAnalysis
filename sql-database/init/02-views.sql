CREATE VIEW denormalized_clusters AS
SELECT
    boards.main AS mainboard,
    boards.side AS sideboard,
    top.cards AS top_cards,
    clusters.id,
    clusters.size,
    tournaments.cardset
FROM clusters
INNER JOIN decks ON clusters.centroid = decks.id
INNER JOIN tournaments ON decks.tournament = tournaments.id
INNER JOIN
    (SELECT
        array_agg(name) FILTER (WHERE inmainboard) AS main,
        array_agg(name) FILTER (WHERE NOT inmainboard) AS side,
        deck
    FROM cards
    GROUP BY deck) boards
ON decks.id = boards.deck
INNER JOIN
    (SELECT
        array_agg(name ORDER BY frequency DESC) AS cards,
        cluster
    FROM top_cards
    GROUP BY cluster) top
ON clusters.id = top.cluster;