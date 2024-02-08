# study-management

This is the main Repo for the Study Management code.

The infrastructure lives over in https://github.com/PA-NIHR-CRN/study-management-infrastructure
Study Management runs in a Lambda function and requests to it go via API Gateway.

An instance of the service is deployed to each of dev/test/uat/oat/prod.

---

## GitHub Actions

A handful of GitHub Actions workflows are defined. These are described below:

- pr.yml - Build and test but don't deploy the Study Managment function (NIHR.StudyManagement.API). This is executed on pull request as part of the CI pipeline.
- deploy_dev.yml - Build, test and deploy the Study Managment function (NIHR.StudyManagement.API). This is a manually executed workflow to build and deploy the function in DEV from any branch for testing.
- sq.yml - Sonarcloud coverage scanner for pull requests.
- staged_deployment.yml - Build, test and deploy the Study Managment function (NIHR.StudyManagement.API) to all environments via staged deployments, Dev will deploy automatically while Test, Uat, Oat and Prod will require approvals at each stage.
- PROD REQUIRES CAB APPROVAL BEFORE DEPLOYMENT

