const { Pool } = require('pg');

const pool = new Pool({
    user: 'api',
    host: 'db',
    database: 'postgres',
    password: process.env.PASSWORD,
    port: 5432
});

pool.on('error', (err, client) => {
    console.error('Error: ', err)
});

async function dbquery(query, parameters){
    try {
        const client = await pool.connect();
        let result = await client.query(query, parameters);
        client.release();
        return { res: result.rows, status: 200 };
    }
    catch(err) {
        return { status: 500 };
    }
}

exports.previews = async function previews(cardset) {
    const query = 'SELECT id, top_cards FROM denormalized_clusters WHERE cardset = $1 ORDER BY size DESC;';
    parameters = [ cardset ];
    return await dbquery(query, parameters);
}

exports.cluster = async function cluster(id) {
    const query = 'SELECT mainboard, sideboard FROM denormalized_clusters WHERE id = $1;';
    parameters = [ id ];
    return await dbquery(query, parameters);
}

exports.cardsets = async function cardsets() {
    const query = 'SELECT DISTINCT cardset FROM tournaments;';
    return await dbquery(query, []);
}