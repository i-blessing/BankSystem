# BankSystem

## Set up

Clone repo with git

## Start
```
Navigate to repo folder
cd BankSystem.API
docker-compose build
docker-compose up
```

## Browse
Navigate to https://localhost:5001/swagger/index.html

## Swagger
### Bearer token
Generate a Bearer token but executing the /api/user endpoint and use the following credentials to generate a token:

username - 'test'
password - 'password'

### Authenticate
Click on the 'Authorize' button and enter the following value: Bearer your-generated-token

###
When authenticated all the endpoints on the Account controller should work.
