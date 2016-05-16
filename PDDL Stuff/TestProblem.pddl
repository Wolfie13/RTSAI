(define (problem test_problem)
	(:domain ai_game)
	
	(:objects
		
		person1 - person
		person2 - person
		
		place1 - place
		place2 - place	
		place3 - place
		quarry - place
		oremine - place
		coalmine - place

		forest1 - forest
	)
	
	(:init
		
		(at person1 place1)
		(at person2 place2)	
		(has-quarry quarry)
		(has-oremine oremine)
		(has-coalmine coalmine)
		(= (time) 0)		
	)
	
	(:goal
		
		(and
			(has-school place3)			
		)
	)
)