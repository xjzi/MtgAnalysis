# Metagame Analysis of Magic: The Gathering

In Magic, designing a powerful deck is often more complex than the main game, so many websites have been built to make this process easier. As a Magic player, I'm mostly satisfied with these sites, but as a programmer I wonder how they were made.

This project is another website to help with Magic, with two differences.
- People can learn how it was built.
- People can add new few features.

Each component of this project was put in a seperate directory.
- `/sql-database`: Postgres database which contains decks and the analysis.
- `/dotnet-scraper`: Parses [tournament archives](https://magic.wizards.com/en/content/deck-lists-magic-online-products-game-info) and writes decks to the database.
- `/r-analysis`: Groups similar decks from the scraper and writes the groups to the database.
- `/node-api`: Queries the database and returns JSON for the frontend to use.
- `/react-frontend`: User interface for viewing the analysis by querying the API.
- `/kubernetes-deployment`: Some config files for deploying this project with Kubernetes.
- `/proxy-database`: A docker compose for starting a database locally.

The website is currently very minimal, and I would need to add a lot more for it to be able to compete with other websites for Magic, but that was never my goal. I might improve it later, but for now I am tired of it. Feel free to create a pull request if you want to take things into your own hands.

This is my first large organized project, and the challenges of completing it will affect the way I plan out projects in the future. Getting a working prototype as fast as possible and then upgrading it incrementally is a great way to stay motivated and adaptable to any problems that come up.

This is also my first project that uses more than C#, and using the right tool for the job is now much more important to me than using the tools I already know. Mastery in only one technology is not enough to be a knowledgeable programmer, and learning about new technologies can give insights into what has already been mastered.

# Website Status

Hosting a website gets pretty expensive over time, so the live version will be stopped at the same time development is stopped. However, a few html files have been saved to give an idea of what the website used to look like. None of the links will work and the card details in theme page will not update, but with some imagination it's easy to understand how the live website worked.
- https://poultryghast.github.io/MtgAnalysis/home
- https://poultryghast.github.io/MtgAnalysis/cardset
- https://poultryghast.github.io/MtgAnalysis/theme