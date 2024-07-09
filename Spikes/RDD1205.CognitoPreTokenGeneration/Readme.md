# RDD-1205 Spike demonstration code
## Overview
This application code is a simple lambda function to demonstrate the AWS Cognito Lambda trigger "pre-token generation"

## Usage
 * Deploy lambda function to AWS
	* In AWS Cognito console, select "User Pool Properties" within your given user pool
	* Add Lambda trigger (Version2) with a trigger type of "Authentication". Under "Authentication" select "pre token generation trigger" and select your lambda function above

Sign-in to the hosted UI should now return an access token that includes the new claims as added by our demo lambda function.