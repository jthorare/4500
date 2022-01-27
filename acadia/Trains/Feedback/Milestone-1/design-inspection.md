Pair: acadia

Commit: 4bd02b738b86134beb862756f36087705b1dabab

Score: 17/30

Grader: Alanna Pasco

**plan-analysis: 4/10**

-1 pt "min/max?" is not a complete sentence<br>
-5 pt Question 4 requires additional explanation as the question is incomprehensible. To clear any confusion please see Trains.Com, a Plan: "At the moment, we anticipate the use of JSON messages over TCP for the actual communication between the server and the clients." This refers to *all* communication between the server and the client.

Evaluation criteria:<br>
- Are questions well-formed?
- Are there spelling and grammar mistakes?

Additional comments:<br>
To answer question three, destinations are worth ten points. This is not specified in the overview section because it is an overview.

**map-design: 13/20**

Evaluation:<br>
+5 pts: data structure constructors from your chosen PL (HashMap)

-5 pts: lacking types or type-like descriptions of how the data structure is used. What data type is a place? a color? an owner?

+5 pts: includes interpretation and assumptions

+3 pts: allows connectivity functionality based on those

-2 pts: for not specifying how to go about rendering a visual representation given the provided data definition

Additional comments:<br>
At the start of a game, connections will not have an owner. How will you represent the lack of an owner in the Tuple(length, owner) of a connection?
