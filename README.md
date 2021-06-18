# MtgAnalysis
A web scraper gets decks and ranks from Wizards of the Coast [game archives.](https://magic.wizards.com/en/content/deck-lists-magic-online-products-game-info)
Multiple cardsets and gametypes are scraped, but only challenges are ranked, so there is a small amount of win/loss data. The results are inserted into a Postgres SQL database.
The results are used to cluster decks into archetypes, based on which cards they have in common. Some other statistics are also computed, and then they are added back to the database. 
A Node.js REST API exposes some of these statistics which are used by the React.js frontend.

# Configuration
The scraper and processing read passwords for database access from the environment variable PASSWORD. The database also uses environment variables SCRAPER and ANALYSIS for the passwords of those users.
