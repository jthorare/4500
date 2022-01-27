Pair: acadia

Commit: 5e42fa388acec706b81d9a30900b39f93db6c46e

Score: 130/145

Grader: Eshwari

`strategy`

95/100

- 20/20 pts for proper organization (an interface for strategies, abstraction over strategy functionality, concrete implementations of hold-10 and buy-now)
  - 10/10 for the specific classes
  - 10/10 for the common buy algorithm factored out
- 0/10 BONUS points if the README or interface module/file contains a diagram for the organization
- 20/20 pts for signatures and purpose statements of
  - 10/10 pts: a choose-destinations method
  - 10/10 pts: a choose-action method
- 20/20 pts for >=1 unit tests of the choose-destination for hold-10
- 20/20 pts for unit tests of the choose-action for buy-now (>= 1 for checking "acquire a connection", >= 1 for checking "give me more cards")
- 16/20 pts for factoring out
  - 8/10 pts: lexical order of destinations: sig/purp/unit
    - 2 pts: No unit test
  - 8/10 pts: lexical order of connections: sig/purp/unit
    - 2 pts: No unit test

-1 pt: Your unit tests for your strategies are good in that you have separate tests for each strategy, but you really should separate each test case further. For example, instead of just having one 'test buy now' conduct turn unit test, you should have separate unit tests for each case: one separate unit test for "buy now strategy, give me more cards", one separate unit test for "buy now strategy, acquire a connection", at the bare minimum...other scenarios you want to test should be in their own separate test. Always separate cases.

`player-protocol`
  
5/15
  
- 0/5 pts `setup` call first
- 0/5 pts `pick` call separate second, or explicitly included in `setup`
- 5/5 pts `take-turn` call follows
- It looks like pick destinations is coming before setup.
- Avoid using "they" to refer to components. Use "it".
