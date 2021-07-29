#!/bin/bash
set -e

psql --username postgres <<-EOSQL
    CREATE ROLE scraper WITH PASSWORD '$SCRAPER' LOGIN;
	GRANT SELECT, INSERT ON TABLE tournaments TO scraper;
	GRANT SELECT, INSERT ON TABLE decks TO scraper;
	GRANT SELECT, INSERT ON TABLE games TO scraper;
	GRANT SELECT, INSERT ON TABLE cards TO scraper;
	GRANT SELECT, INSERT, DELETE ON TABLE tmp_tournaments to scraper;
	
	CREATE ROLE analysis WITH PASSWORD '$ANALYSIS' LOGIN;
	GRANT SELECT ON TABLE tournaments TO analysis;
	GRANT SELECT ON TABLE decks TO analysis;
	GRANT SELECT ON TABLE games TO analysis;
	GRANT SELECT ON TABLE cards TO analysis;
	GRANT INSERT, DELETE ON TABLE top_cards TO analysis;
	GRANT INSERT, DELETE ON TABLE clusters TO analysis;
	GRANT USAGE ON SEQUENCE clusters_id_seq TO analysis;

	CREATE ROLE api WITH PASSWORD '$API' LOGIN;
	GRANT SELECT ON TABLE cards TO api;
	GRANT SELECT ON TABLE clusters TO api;
	GRANT SELECT ON TABLE top_cards TO api;
	GRANT SELECT ON TABLE decks to api;
	GRANT SELECT ON TABLE tournaments to api;
	GRANT SELECT ON TABLE denormalized_clusters to api;
EOSQL