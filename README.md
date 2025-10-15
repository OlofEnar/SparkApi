This is the backend api I built during my internship at Spark Vision.
They wanted an internal service that daily fetched anonymous user data from their Snowflake cloud and stored it aggregated in a local postgres db,
so it could later be used to display business insight and statistics in the frontend.

It's built with ASP Core and using Dapper as ORM together with an Postgres DB.
