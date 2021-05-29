const port = 1337;
const express =  require('express');
const app = express();
const sql = require('./sql');

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static('public'))

app.get("/api/clusters", async (req, res) => {
    const api = await sql.clusterPreviews();
    res.json(api);
});

app.listen(port);