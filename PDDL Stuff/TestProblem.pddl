(define (problem test_problem)
	(:domain ai_game)
	
	(:objects
		
		person1 - person
		person2 - person
		
		place1 - place
		place2 - place	

		forest1 - forest
	)
	
	(:init
		
		(at person1 place1)
		(at person2 place2)	
		(has-skill person1)		
	)
	
	(:goal
		
		(and
			(has-timber person1)			
		)
	)
)