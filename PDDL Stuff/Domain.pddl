(define (domain ai_game)
	(:requirements :strips)
	(:types place resource person cart)
	
	(:predicates
		
		(has-sawmill ?p - place)
		(has-smelter ?p - place)
		(has-school ?p - place)
		
	)
	
	(:functions
	 (space-in ?c - cart)
	
	(:constants coal ore timber stone iron wood)

	;; Actions Load - Unload
	(:action load
	 :parameters (?c - cart ?p - place ?r - resource)
	 :precondition (and ()
	)
) 