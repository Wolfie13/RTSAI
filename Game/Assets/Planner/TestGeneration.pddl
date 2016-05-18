(define (problem team_problem)
(:domain ai_game)
(:objects

Person - person
Human4 - person

oreresource - resource
coalresource - resource
)
(:init
(ore_resource oreresource)
(coal_resource coalresource)
(has-storage)
(= (time) 0)
(= (wood) 0)
(= (iron) 0)
(= (timber) 0)
(= (stored-ore) 0)
(= (stored-coal) 0)
(= (rifles) 0)
(= (riflemen) 0)
(= (stone) 0)
(= (population) 2)
(= (min_resource) 0)
(= (ore oreresource) 5)
(= (coal coalresource) 5)
)
(:goal
(and
(has-school)
(= (riflemen) 5)
(= (iron) 10)
)
)
)
