(define (problem team_problem)
(:domain ai_game)
(:objects
forest1 - forest
human1 - person
human1place - place
human2 - person
human2place - place
oreresource - resource
coalresource - resource
quarry - place
)
(:init
(at human1 human1place)
(at human2 human2place)
(ore_resource oreresource)
(coal_resource coalresource)
(has-building oreresource)
(has-building coalresource)
(has-quarry quarry)
(= (time) 0)
(= (min_resource) 0)
(= (ore oreresource) 5)
(= (coal coalresource) 5)
)
(:goal
(and
(has-wood human1)
)
)
)
