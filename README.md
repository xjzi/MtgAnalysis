# MtgAnalysis
A web scraper gets decks and ranks from Wizards of the Coast [game archives.](https://magic.wizards.com/en/articles/archive/mtgo-standings/modern-challenge-2021-02-01)
From the decks and ranks, we can identify which decks beat which other decks because last place always plays first place, second-to-last plays second, and so on.
This is uploaded to a Postgres SQL database whose machine is also hosting the crontab to check for tournaments on a daily basis and hosting the Selenium Remote Browser to load the required Javascript.
The data from the SQL database is now used to cluster decks into archetypes, based on which cards they have in common. Some other statistics are also computed, and then they are added back to the database. 
Then the Blazor frontend provides a JSON api for the business objects, such as cards and clusters. This api is used by the client to browse the statistics.
