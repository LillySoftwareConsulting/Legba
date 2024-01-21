# Contribution Notes
The repository owner and maintainer is: [Scott Lilly](https://github.com/ScottLilly)

## Issues workflow
- Please report new issues or feature requests here: [Issues](https://github.com/LillySoftwareConsulting/Legba/issues)
- New issues will be added to Parking Lot in the task board
- Tasks will be triaged by the repository owner
  - If this project gets large or popular, I may add more maintainers
 
## How to contribute
- Only pull work from the "Approved" column on the [project task board](https://github.com/orgs/LillySoftwareConsulting/projects/2/views/1)
  - Please check with maintainer first if you want to work on an issue from the "Future Work" column
- Assign the issue to yourself when pulling it into "In Progress"
- Please use issue comments for your questions, so we have a history of why something was done the way it was
- Create a new branch for your changes and submit a pull request to have it merged
  - Ensure the pull request passes the build action before submitting it

## Coding standards
- Please follow existing coding standards as much as possible
  - Contact repository maintainer if you want to suggest an improvement
- Names (classes, methods, variables, etc.) should be descriptive
  - Long names are definitely OK, if they add clarity
- Try to generally follow SOLID principles
- Low cyclomatic complexity is generally preferred
- Use System.Text.Json when working with JSON (not Newtonsoft)

## Code of Conduct
- Don't be a jerk. Life is tough enough for many of us.
