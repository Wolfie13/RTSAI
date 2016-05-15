(define (domain ai_game)
	(:requirements :strips :typing)
	(:types 	
		place - object		 
		person - object
		building - place
		forest - place
	)
	
	(:predicates
		(at ?person - person ?p - place)
		
		(has-sawmill ?p - place)
		(has-smelter ?p - place)
		(has-school ?p - place)
		(has-storage ?p - place)
		(has-mine ?p - place)
		(has-quary ?p - place)
		(has-blacksmith ?p - place)
		(has-barracks ?p - place)
		(has-skill ?person - person)
		(has-riflemanskill ?person - person)

		(is-carpenter ?person - person)
		(is-blacksmith ?person - person)
		(is-lumberjack ?person - person)
		(is-teacher ?person - person)
		(is-miner ?person - person)
		(is-rifleman ?person - person)			
	)
	
	;; ------------- ACTIONS ---------- ;;
	
	(:action move
			:parameters (?person - person ?pl-from ?pl-to - place)
			:precondition (and (at ?person ?pl-from))
			:effect(and(at ?person ?pl-to) (not (at ?person ?pl-from)))
	)

	;;------------- Training ------------- ;;
	
	(:action simpleEducate
			:parameters(?person - person ?student - person ?place - place)
			:precondition(and(at ?person ?place) (at ?student ?place) (not(has-skill ?student)))
			:effect(and (has-skill ?student))
	)
	
	(:action schoolEducate
			:parameters(?student - person ?teacher - person ?place - place)
			:precondition(and(at ?student ?place) (at ?teacher ?place) (is-teacher ?teacher) (has-school ?place))
			:effect(and (has-skill ?student))
	)
	
	(:action barracksEducate
			:parameters(?student - person ?teacher - person ?place - place)
			:precondition(and(at ?student ?place) (at ?teacher ?place) (is-teacher ?teacher) (has-barracks ?place))
			:effect(and (has-riflemanskill ?student))
	)
	
	(:action trainCarpenter
			:parameters(?person - person)
			:precondition(and(has-skill ?person) (not(is-carpenter ?person)))
			:effect(and (is-carpenter ?person) (not(has-skill ?person)))
	)
	
	(:action trainBlacksmith
			:parameters(?person - person)
			:precondition(and(has-skill ?person) (not(is-blacksmith ?person)))
			:effect(and (is-blacksmith ?person) (not(has-skill ?person)))
	)
	
	(:action trainLumberjack
			:parameters(?person - person)
			:precondition(and(has-skill ?person) (not(is-lumberjack ?person)))
			:effect(and (is-lumberjack ?person) (not(has-skill ?person)))
	)
	
	(:action trainTeacher
			:parameters(?person - person)
			:precondition(and(has-skill ?person) (not(is-teacher ?person)))
			:effect(and (is-teacher ?person) (not(has-skill ?person)))
	)

	(:action trainMiner
			:parameters(?person - person)
			:precondition(and(has-skill ?person) (not(is-miner ?person)))
			:effect(and (is-miner ?person) (not(has-skill ?person)))
	)
	
	(:action trainRifleman
			:parameters(?person - person)
			:precondition(and(has-riflemanskill ?person) (not(is-rifleman ? person)))
			:effect(and (is-rifleman ?person) (not(has-riflemanskill ?person)))
	)
)	
