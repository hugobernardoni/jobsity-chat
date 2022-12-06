# Jobsity .NET Challenge
## Financial chat App

## Description
This project is designed to test your knowledge of back-end web technologies, specifically in 
.NET and assess your ability to create back-end products with attention to details, standards,
and reusability.


## Assignment
The goal of this exercise is to create a simple browser-based chat application using .NET.

This application should allow several users to talk in a chatroom and also to get stock quotes
from an API using a specific command.

## Requirements
You must have *docker* installed on your operating system (Linux, Windows or Mac).  

# Steps to run the application

### Run the command:
- ` docker-compose -f ./API/docker-compose.yml up --build` 

### After, access http://localhost:8200 into a web-browser and start to using the **System**.


# Features included

- Multiple rooms
- Messages ordered by their `Timestamps`
- Command to get stocks prices via `/stock=stock_code`
- Messages saved in database
- Handling errors/commands not allowed
- Run the application via `docker.compose.yml`
- Limit of 50 messages per chat
- Authentication of users with .NET identity
- Unit Tests
