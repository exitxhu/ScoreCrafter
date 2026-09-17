score crafter

a simple solution for grading and scoring customers based on dynamic rules

Solution contents:

1- ScoreCrafter.Domain:

HighLevel Abstraction, Entities, Dtos, Exception, Invariants, Enums, ValueObjects

2- ScoreCrafter.Application:

Domain implementation 

3- ScoreCrafter.Infrastructure

LowLevel dependencies implementations like db, cache etc.

4- ScoreCrafter.Api:

Edge of domain, Rest, gRpc etc

5- ScoreCrafter.Tests

Integrations and Unit tests

6- ScoreCrafter.Externals

Mocking expected co-services


Architechtur Design Notes:

* Dynamic Formula:
	In a real system we can not risk hardcoding such important piece of action, cuz of regular updateds.
	regular down time and bulking code eventually bring us to a big ball of mud, so i decided to use my experience in a dynamic marketing system and bring a dynamic formula to the system.
	as this Customer club domain extends, the value of this approach will be more clear.