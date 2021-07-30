const port = 1337;
const express =  require('express');
const cors = require('cors');
const app = express();
const sql = require('./sql');

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static('public'));
app.use(cors());

app.get("/api/clusters/:cardset", async (req, res) => {
    const api = await sql.previews(req.params.cardset);
    res.status(api.status);
    res.json(api.res);
});

app.get("/api/cluster/:id", async (req, res) => {
    const api = await sql.cluster(req.params.id);
    res.status(api.status);
    res.json(api.res);
});

app.get("/api/cardsets", async (req, res) => {
    const api = await sql.cardsets();
    res.status(api.status);
    res.json(api.res);
});

app.listen(port);