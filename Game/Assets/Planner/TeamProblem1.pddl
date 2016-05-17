(define (problem test_problem)
	(:domain ai_game)
	
	(:objects
		
		person1 - person
		person2 - person
		person3 - person
		
		place1 - place
		place2 - place	
		place3 - place
		quarry - place		
		coalmine - place
		oreresource - resource
		coalresource - resource

		forest1 - forest
	)
	
	(:init
		
		(at person1 place1)
		(at person2 place2)	
		(has-quarry quarry)					
		(has-building quarry)
		(ore_resource oreresource)
		(coal_resource coalresource)
		(has-building oreresource)
		(has-building coalresource)
		(= (time) 0)
		(= (min_resource) 0)
		(= (ore oreresource) 1)
		(= (coal coalresource) 1)
			
	)
	
	(:goal
		
		(and
			(has-timber person1)			
		)
	)
)