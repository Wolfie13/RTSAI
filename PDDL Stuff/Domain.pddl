(define (domain ai_game)
	(:requirements :strips :typing :fluents)
	(:types 	
		place - object		 
		person - object
		building - place
		forest - place
	)
	
	(:functions
		(wood)
		(timber)
		(iron)
		(ore)
		(stone)
		(time)
	)
	
	(:predicates
		(at ?person - person ?p - place)
		
		(has-sawmill ?p - place)
		(has-smelter ?p - place)
		(has-school ?p - place)
		(has-storage ?p - place)
		(has-oremine ?p - place)
		(has-coalmine ?p - place)
		(has-quarry ?p - place)
		(has-blacksmith ?p - place)
		(has-barracks ?p - place)
		(has-skill ?person - person)
		(has-riflemanskill ?person - person)
		
		(is-turfhut ?p - place)
		
		(has-timber ?person - person)
		(has-wood ?person - person)
		(has-coal ?person - person)
		(has-ore ?person - person)
		(has-iron ?person - person)
		(has-stone ?person - person)
		(has-axe ?person - person)
		
		(stored-timber ?storage - place)

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
			:effect(and (has-skill ?student) (increase (time) 100))
	)
	
	(:action schoolEducate
			:parameters(?student - person ?teacher - person ?place - place)
			:precondition(and(at ?student ?place) (at ?teacher ?place) (is-teacher ?teacher) (has-school ?place))
			:effect(and (has-skill ?student) (increase (time) 50))
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
			:precondition(and(has-riflemanskill ?person) (not(is-rifleman ?person)))
			:effect(and (is-rifleman ?person) (not(has-riflemanskill ?person)))
	)
	
	;; --------- Acquire Resource --------- ;;
	
	(:action cutTree
			:parameters(?lumberjack - person ?tree - forest)
			:precondition(and(is-lumberjack ?lumberjack) (at ?lumberjack ?tree))
			:effect(has-timber ?lumberjack)
	)
	
	(:action mineOre
			:parameters(?miner - person ?mine - place)
			:precondition(and(is-miner ?miner) (at ?miner ?mine) (has-oremine ?mine))
			:effect(has-ore ?miner)
	)
	
	(:action mineCoal
			:parameters(?miner - person ?mine - place)
			:precondition(and(is-miner ?miner) (at ?miner ?mine) (has-coalmine ?mine))
			:effect(has-coal ?miner)
	)

	(:action cutStone
			:parameters(?stonecutter - person ?quarry - place)
			:precondition(and(at ?stonecutter ?quarry) (has-quarry ?quarry))
			:effect(has-stone ?stonecutter)
	)
	
	
	(:action produceWood
			:parameters(?person - person ?sawmill - place)
			:precondition(and(has-timber ?person) (at ?person ?sawmill) (has-sawmill ?sawmill))
			:effect(has-wood ?person)
	)
	
	(:action produceIron
			:parameters(?person - person ?smelter - place)
			:precondition(and(has-coal ?person) (has-ore ?person) (at ?person ?smelter) (has-smelter ?smelter))
			:effect(has-iron ?person)
	)
	
	(:action makeTool
			:parameters(?blacksmith - person ?place - place)
			:precondition(and(at ?blacksmith ?place) (is-blacksmith ?blacksmith) (has-smelter ?place))
			:effect(has-axe ?blacksmith)
	)
	
	;; --------- Build Building ---------- ;;
	
	(:action buildTurfHut
			:parameters(?person - person ?place - place)
			:precondition(and(at ?person ?place) (not(is-turfhut ?place)))
			:effect(is-turfhut ?place)
	)
	
	
	;; --------- Store Resource ---------- ;;
	
	(:action storeTimber
			:parameters(?person - person ?storage - place)
			:precondition(and(at ?person ?storage) (has-storage ?storage) (has-timber ?person))
			:effect(and (stored-timber ?storage) (increase (wood) 1))
	)
			
)	
