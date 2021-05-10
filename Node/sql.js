const { Pool } = require('pg');

module.exports.clusterPreviews = clusterPreviews;

const pool = new Pool({
    user: 'app',
    host: process.env.mtganalysis_ip,
    database: 'tournaments',
    password: process.env.mtganalysis_password,
    port: 5432
});

pool.on('error', (err, client) => {
    console.error('Error: ', err)
});


async function clusterPreviews() {
    const clustersQuery = 'select id, frequency from clusters order by id;';
    const highlightsQuery = 'select * from highlightcards;';

    try {
        const client = await pool.connect();
        let clustersResult = await client.query(clustersQuery);
        let highlightsResult = await client.query(highlightsQuery);
        var clusters = clustersResult.rows.map((obj) => {
            obj.highlights = [];
            return obj;
        }); 
        var highlights = highlightsResult.rows;
    }
    catch(err) {
        return "SQL Database Error";
    }

    for (var i = 0; i < highlights.length; i++) {
        clusters[highlights[i].cluster].highlights.push({ card: highlights[i].card, frequency: highlights[i].frequency });
    }

    return clusters;
}