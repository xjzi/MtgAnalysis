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
    const query = 'select * from previews;';

    try {
        const client = await pool.connect();
        let result = await client.query(query);
        client.release();
        return result.rows;
    }
    catch(err) {
        return "SQL Database Error";
    }
}