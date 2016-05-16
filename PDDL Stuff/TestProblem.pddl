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

		forest1 - forest
	)
	
	(:init
		
		(at person1 place1)
		(at person2 place2)	
		(has-quarry quarry)					
		(has-coalmine coalmine)
		(has-building coalmine)
		(has-building quarry)
		(= (time) 0)		
	)
	
	(:goal
		
		(and
			(has-school place3)			
		)
	)
)