(define (problem team_problem)
(:domain ai_game)
(:objects
Person00 - person
Person01 - person
oreresource - resource
coalresource - resource
)
(:init
(has-storage)
(has-quarry)
(has-smelter)
(has-forge)
(has-labourer)
(has-carpenter)
(has-blacksmith)
(has-teacher)
(has-miner)
(has-lumberjack)
(has-rifleman)
(ore_resource oreresource)
(coal_resource coalresource)
(= (time) 0)
(= (wood) 1)
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
(has-forge)
)
)
)
