# Introduction
Analyzes and displays common themes for tournament decks in Magic the Gathering
## External Libraries
- C# scraper: anglesharp, npgsql, tomlyn
- Node API: cors, express, pg
- R Analysis: DBI, analogue, stats, RPostgres, dbscan

## Structure
- `/sql-database`: Postgres database which contains decks and the analysis.
- `/dotnet-scraper`: Parses [tournament archives](https://magic.wizards.com/en/content/deck-lists-magic-online-products-game-info) and writes decks to the database.
- `/r-analysis`: Groups similar decks from the scraper and writes the groups to the database.
- `/node-api`: Queries the database and returns JSON for the frontend to use.
- `/react-frontend`: User interface for viewing API data in a graphical way.
- `/kubernetes-deployment`: Config files for deploying this project with Kubernetes.
- `/proxy-database`: A docker-compose file for starting a database locally.

## Demonstration
There are currently a few static pages which give an idea of the website, but the links will not work.
- https://xjzi.github.io/MtgAnalysis/home
- https://xjzi.github.io/MtgAnalysis/cardset
- https://xjzi.github.io/MtgAnalysis/theme

## Motivation
- Magic the Gathering is a complex game
- I was curious about how other analysis websites worked
- I already knew C#
- R has lots of mature libraries for statistics
- Javascript is the web standard
- React is popular and easy to use
- Kubernetes is overcomplicated but it seemed like a good way to run a cron job
- Docker is good for declaratively managing program dependencies
- NodeJS is the standard for web API's

## Future
Servers get pretty expensive. All I currently have is an SQL dump of the database after a few months of scraping. The best way to share the website without paying for a web server is hosting static files on github. I would use Astro Build to generate these files. I spent a lot of time on this project, so I want to display it well, but I'm also tired of working on it.

I could improve a few parts of this project, if I wanted to keep it online.
- Render frontend with Astro Build
- Remove password protection between containers in PostgresSQL
- Try out some continous integration
