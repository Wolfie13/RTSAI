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
(has-building storage)
(has-building forest1)
(= (time) 0)
(= (wood) 0)
(= (timber) 0)
(= (stored-ore) 0)
(= (stored-coal) 0)
(= (iron) 0)
(= (stone) 0)
(= (rifles) 0)
(= (riflemen) 0)
(= (population) 2)
(= (min_resource) 0)
(= (ore oreresource) 5)
(= (coal coalresource) 6)
)
(:goal
(and
(= (riflemen) 2)
(has-building human1place)
(has-school human1place)
)
)
)
