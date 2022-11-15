# Functional Requirements

* As a parameter, it should receive the path to a Git repository that resides in a local directory
* Should print different outputs depending on different modes
    * "Commit frequency: Should print amounts of commits in total for all days grouped by day"
    * "Commit Author Mode: Should print the amount of commits by a given author grouped by amount per day"
* All results are printed in StdOut
* Results from queries must be stored in a persistent form.
* A local copy of the chosen repository should be created in a temporary folder if it does not already exist.
* The application must be represented as a web-application.

# Non-Functional Requirements

* Needs to written in C#
* Has to use LibGit2Sharp
* When analyzing a repository that already has results in the database
    * Update the database to reflect the newest state of the repository if the state has changed.
    * If the current state of the repository matches the one from the latest analysis simply return said analysis.
* The web-application should expose a REST API.
* The REST API shall return the analysis results via JSON objects.