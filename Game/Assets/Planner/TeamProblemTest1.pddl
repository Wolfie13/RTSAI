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
storage - place
)
(:init
(at human1 human1place)
(at human2 human2place)
(ore_resource oreresource)
(coal_resource coalresource)
(has-building oreresource)
(has-building coalresource)
(has-quarry quarry)
(has-storage storage)
(= (time) 0)
(= (wood) 0)
(= (timber) 0)
(= (stored-ore) 0)
(= (stored-iron) 0)
(= (min_resource) 0)
(= (ore oreresource) 5)
(= (coal coalresource) 5)
)
(:goal
(and
(= (wood) 5)
(= (timber) 10)
(= (stored-ore) 5)
)
)
)
