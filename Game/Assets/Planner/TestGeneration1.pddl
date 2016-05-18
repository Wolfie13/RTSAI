(define (problem team_problem)
(:domain ai_game)
(:objects
forest1 - forest
Human3 - person
Human3place - place
Human4 - person
Human4place - place
oreresource - resource
coalresource - resource
quarry - place
storage - place
)
(:init
(at Human3 Human3place)
(at Human4 Human4place)
(ore_resource oreresource)
(coal_resource coalresource)
(has-building oreresource)
(has-building coalresource)
(has-storage storage)
(has-building storage)
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
(has-sawmill Human3place)

)
)
)
