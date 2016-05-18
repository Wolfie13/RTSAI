(define (domain ai_game)
	(:requirements :strips :typing :fluents :conditional-effects)
	(:types 	
		place - object		 
		person - object
		building - place
		forest - place
		resource - place		
		
	)
	
	(:functions
		(wood)
		(timber)
		(iron)
		(stone)
		(rifles)
		(stored-ore)
		(stored-coal)	
		(ore ?p - place)
		(coal ?p - place)
		(min_resource)		
		(time)
		(population)
	)
	
	(:predicates
		(at ?person - person ?p - place)
		(has-building ?p - place)
		
		(has-house ?p - place)
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
		(has-rifle ?person - person)
		
		(ore_resource ?p - place)
		(coal_resource ?p - place)
		
		(has-turfhut ?p - place)
		
		(has-timber ?person - person)
		(has-wood ?person - person)
		(has-coal ?person - person)
		(has-ore ?person - person)
		(has-iron ?person - person)
		(has-stone ?person - person)
		(has-axe ?person - person)

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
	
	(:action reproduceTurfHut
			:parameters (?person1 - person ?person2 - person ?place - place)
			:precondition(and (at ?person1 ?place) (at ?person2 ?place) (not (= ?person1 ?person2)) (has-turfhut ?place))
			:effect(and (increase (population) 1))
	)
	
	(:action reproduceHouse
			:parameters(?person1 - person ?person2 - person ?place - place)
			:precondition(and (at ?person1 ?place) (at ?person2 ?place) (not (= ?person1 ?person2)) (has-house ?place))
			:effect(and (increase (population) 1))
	)

	;;------------- Training ------------- ;;
	
	(:action simpleEducate
			:parameters(?person - person ?student - person ?place - place)
			:precondition(and(at ?person ?place) (at ?student ?place) (not(has-skill ?student)) (not(= ?person ?student)))
			:effect(and (has-skill ?student) (increase (time) 100))
	)
	
	(:action schoolEducate
			:parameters(?student - person ?teacher - person ?place - place)
			:precondition(and(at ?student ?place) (at ?teacher ?place) (is-teacher ?teacher) (has-school ?place) (not(has-skill ?student)))
			:effect(and (has-skill ?student) (increase (time) 50))
	)
	
	(:action barracksEducate
			:parameters(?student - person ?teacher - person ?place - place)
			:precondition(and(at ?student ?place) (at ?teacher ?place) (is-teacher ?teacher) (has-barracks ?place))
			:effect(and (has-riflemanskill ?student) (increase (time) 30))
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
			:precondition(and(has-riflemanskill ?person) (not(is-rifleman ?person)) (>= (rifles) 1))
			:effect(and (is-rifleman ?person) (not(has-riflemanskill ?person)))
	)
	
	;; --------- Acquire Resource --------- ;;
	
	(:action cutTree
			:parameters(?lumberjack - person ?tree - forest)
			:precondition(and(is-lumberjack ?lumberjack) (at ?lumberjack ?tree))
			:effect(has-timber ?lumberjack)
	)
	
	(:action simpleMineOre
			:parameters(?person - person ?resource - resource)
			:precondition(and(> (ore ?resource) (min_resource)))
			:effect(and(has-ore ?person) (decrease (ore ?resource) 1))
	)
	
	(:action simpleMineCoal
			:parameters(?person - person ?resource - resource)
			:precondition(and(> (coal ?resource) (min_resource)))
			:effect(and(has-coal ?person) (decrease (coal ?resource) 1))
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
			:precondition(and (at ?person ?sawmill) (has-sawmill ?sawmill) (>= (timber) 1))
			:effect(and(has-wood ?person) (decrease (timber) 1))
	)
	
	(:action produceIron
			:parameters(?person - person ?smelter - place)
			:precondition(and(>= (stored-coal) 1) (>= (stored-ore) 1) (at ?person ?smelter) (has-smelter ?smelter))
			:effect(and(has-iron ?person) (decrease (stored-ore) 1) (decrease (stored-coal) 1))
	)
	
	(:action produceTool
			:parameters(?blacksmith - person ?place - place)
			:precondition(and(at ?blacksmith ?place) (is-blacksmith ?blacksmith) (has-blacksmith ?place))
			:effect(has-rifle ?blacksmith)
	)
	
	;; --------- Build Building ---------- ;;
	
	(:action buildTurfHut
			:parameters(?person - person ?place - place)
			:precondition(and(at ?person ?place) (not(has-turfhut ?place)) (not(has-building ?place)))
			:effect(and(has-turfhut ?place) (has-building ?place))
	)
	
	(:action buildHouse
			:parameters(?carpenter - person ?person - person ?place - place)
			:precondition(and(at ?carpenter ?place) (at ?person ?place) (is-carpenter ?carpenter) (not(has-house ?place)) (not(has-building ?place)) (not(= ?person ?carpenter))
						  (>= (wood) 1))
			:effect(and(has-house ?place) (has-building ?place) (decrease (wood) 1))
	)
	
	(:action buildSmelter
			:parameters(?person - person ?place - place)
			:precondition(and(at ?person ?place) (not(has-smelter ?place)) (not(has-building ?place)) 
						  (>= (stone) 1))
			:effect(and(has-smelter ?place) (has-building ?place) (decrease (stone) 1))
	)
	
	(:action buildSchool
			:parameters(?carpenter - person ?person - person ?place - place)
			:precondition(and(at ?person ?place) (at ?carpenter ?place) (is-carpenter ?carpenter) (not(has-school ?place)) (not(has-building ?place)) (not(= ?person ?carpenter))
						  (>= (iron) 1)
						  (>= (wood) 1)
						  (>= (stone) 1))						  
			:effect(and(has-school ?place) (has-building ?place) (decrease (iron) 1) (decrease (wood) 1) (decrease (stone) 1))
	)
	
	(:action buildSawMill
			:parameters(?person - person ?place - place)
			:precondition(and(at ?person ?place) (not(has-building ?place)) (not(has-sawmill ?place)) 
						  (>= (stone) 1) 
						  (>= (timber) 1) 
						  (>= (iron) 1))
			:effect(and(has-sawmill ?place) (has-building ?place) (decrease (stone) 1) (decrease (iron) 1) (decrease (timber) 1))
	)
	
	(:action buildBlacksmith
			:parameters(?person - person ?place - place)
			:precondition(and(at ?person ?place) (not(has-building ?place)) (not(has-blacksmith ?place)) 
						  (>= (stone) 1) 
						  (>= (timber) 1) 
						  (>= (iron) 1))
			:effect(and(has-blacksmith ?place) (has-building ?place) (decrease (stone) 1) (decrease (iron) 1) (decrease (timber) 1))
	)
	
	(:action buildOreMine
			:parameters(?carpenter - person ?blacksmith - person ?person - person ?resource - resource)
			:precondition(and(is-carpenter ?carpenter) (is-blacksmith ?blacksmith) (at ?carpenter ?resource) (at ?blacksmith ?resource) (ore_resource ?resource) (not(= ?carpenter ?blacksmith)) (not(has-oremine ?resource))
						  (>= (wood) 1)
						  (>= (iron) 1))
			:effect(and(has-oremine ?resource) (has-building ?resource) (decrease (wood) 1) (decrease (iron) 1))
	)
	
	(:action buildCoalMine
			:parameters(?carpenter - person ?blacksmith - person ?person - person ?resource - resource)
			:precondition(and(is-carpenter ?carpenter) (is-blacksmith ?blacksmith) (at ?carpenter ?resource) (at ?blacksmith ?resource) (coal_resource ?resource) (not (= ?carpenter ?blacksmith)) (not(has-coalmine ?resource))
						  (>= (wood) 1)
						  (>= (iron) 1))
			:effect(and(has-coalmine ?resource) (has-building ?resource) (decrease (wood) 1) (decrease (iron) 1))
	)		
	
	(:action buildBarracks
			:parameters(?carpenter - person ?person - person ?place - place)
			:precondition(and(is-carpenter ?carpenter) (at ?carpenter ?place) (at ?person ?place) (not(= ?carpenter ?person)) (not(has-barracks ?place)) (not(has-building ?place))
						  (>= (stone) 1)
						  (>= (wood) 1))
			:effect(and (has-barracks ?place) (has-building ?place) (decrease (stone) 1) (decrease (wood) 1))
	)
	
	;; --------- Store Resource ---------- ;;
	
	(:action storeTimber
			:parameters(?person - person ?storage - place)
			:precondition(and(at ?person ?storage) (has-storage ?storage) (has-timber ?person))
			:effect(and (increase (timber) 1) (not(has-timber ?person)))
	)

	(:action storeWood
			:parameters(?person - person ?storage - place)
			:precondition(and(at ?person ?storage) (has-storage ?storage) (has-wood ?person))
			:effect(and (increase (wood) 1) (not(has-wood ?person)))
	)
	
	(:action storeOre
			:parameters(?person - person ?storage - place)
			:precondition(and(at ?person ?storage) (has-storage ?storage) (has-ore ?person))
			:effect(and (increase (stored-ore) 1) (not(has-ore ?person)))
	)
	
	(:action storeCoal
			:parameters(?person - person ?storage - place)
			:precondition(and(at ?person ?storage) (has-storage ?storage) (has-coal ?person))
			:effect(and (increase (stored-coal) 1) (not(has-coal ?person)))
	)
	
	(:action storeIron
			:parameters(?person - person ?storage - place)
			:precondition(and (at ?person ?storage) (has-storage ?storage) (has-iron ?person))
			:effect(and (increase (iron) 1) (not (has-iron ?person)))
	)
	
	(:action storeStone
			:parameters(?person - person ?storage - place)
			:precondition(and (at ?person ?storage) (has-storage ?storage) (has-stone ?person))
			:effect(and (increase (stone) 1) (not (has-stone ?person)))
	)
	
	;;store Rifle ? best way of doing this?
	(:action storeRifle
			:parameters(?person - person ?storage - place)
			:precondition(and (at ?person ?storage) (has-storage ?storage) (has-rifle ?person))
			:effect(and (increase (rifles) 1) (not (has-rifle ?person)))
	)
)	
