# An introduction to Mongo databases (a document based data store)
----

#### Setting up demo database.
1. For simplicity we are using a docker hosted mongo database. Ensure docker is running linux containers or the setup script will not be called on database instantiation.
2. Build the docker image by calling "docker build -f Dockerfile -t meetup-database --build-arg ADMIN_USERNAME=admin --build-arg ADMIN_PASSWORD=pass --build-arg DATABASE_USERNAME=user --build-arg DATABASE_PASSWORD=pass ."
3. Run the image by calling "docker run -p 8000:27017 meetup-database"

#### Connecting to the database.
In order to connect to the mongo database, there are a few options:
1. Connect using mongo shell (https://www.mongodb.com/download-center/community) using "mongo --username user --password pass --authenticationDatabase meetup --host localhost --port 8000"
2. Connect using mongo compass (https://www.mongodb.com/products/compass) by setting hostname to localhost, port to 8000, authentication to Username / Password, username to user, password to pass and authentication database to meetup.

#### FAQ
1. When running the database container, errors pop up saying that the files being used by mongoimport cannot be found: ensure that the setup.sh script is using linux line feeds, instead of windows ones.
2. I can run the database container, but its empty: ensure that you are running linux containers if you are operating in a windows OS, otherwise the setup scripts will not be invoked.
3. Queries seem slow, checking index usage: from the mongo shell you can run db.park.aggregate([ { $indexStats: { } } ]), which tells when and how many times your indexes have been used.