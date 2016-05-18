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
		(riflemen)
		(population)
	)
	
	(:predicates
		
		
		(has-turfhut)
		(has-house)
		(has-sawmill)
		(has-smelter)
		(has-school)
		(has-storage)
		(has-oremine)
		(has-coalmine)
		(has-quarry)
		(has-blacksmith)
		(has-barracks)
		(has-skill ?person - person)
		(has-riflemanskill ?person - person)
		(has-rifle ?person - person)
		
		(ore_resource ?p - place)
		(coal_resource ?p - place)		
		
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

	(:action reproduceTurfHut
			:parameters ()
			:precondition(has-turfhut)
			:effect(and (increase (population) 1))
	)
	
	(:action reproduceHouse
			:parameters()
			:precondition(has-house)
			:effect(and (increase (population) 2))
	)
	;;------------- Training ------------- ;;
	
	(:action simpleEducate
			:parameters(?person - person ?student - person ?place - place)
			:precondition(and (not(has-skill ?student)) (not(= ?person ?student)))
			:effect(and (has-skill ?student) (increase (time) 100))
	)
	
	(:action schoolEducate
			:parameters(?student - person ?teacher - person ?place - place)
			:precondition(and (is-teacher ?teacher) (has-school) (not(has-skill ?student)))
			:effect(and (has-skill ?student) (increase (time) 50))
	)
	
	(:action barracksEducate
			:parameters(?student - person ?teacher - person ?place - place)
			:precondition(and(is-teacher ?teacher) (has-barracks))
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
			:effect(and (is-rifleman ?person) (not(has-riflemanskill ?person)) (increase (riflemen) 1) (decrease (rifles) 1))
	)
	
	;; --------- Acquire Resource --------- ;;
	
	(:action cutTree
			:parameters(?lumberjack - person)
			:precondition(and(is-lumberjack ?lumberjack))
			:effect(and (has-timber ?lumberjack) (increase (timber) 1))
	)
	
	(:action simpleMineOre
			:parameters(?resource - resource)
			:precondition(and(> (ore ?resource) (min_resource)))
			:effect(and (increase (stored-ore) 1) (decrease (ore ?resource) 1))
	)
	
	(:action simpleMineCoal
			:parameters(?resource - resource)
			:precondition(and(> (coal ?resource) (min_resource)))
			:effect(and(increase(stored-coal) 1) (decrease (coal ?resource) 1))
	)	
	
	(:action mineOre
			:parameters(?miner - person)
			:precondition(and(is-miner ?miner) (has-oremine))
			:effect(increase(stored-ore) 1)
	)
	
	(:action mineCoal
			:parameters(?miner - person)
			:precondition(and(is-miner ?miner)  (has-coalmine))
			:effect(increase(stored-coal) 1)
	)

	(:action cutStone
			:parameters()
			:precondition(and (has-quarry))
			:effect(increase(stone) 1)
	)	
	
	(:action produceWood
			:parameters()
			:precondition(and (has-sawmill) (>= (timber) 1))
			:effect(and(increase (wood) 1) (decrease (timber) 1))
	)
	
	(:action produceIron
			:parameters()
			:precondition(and(>= (stored-coal) 1) (>= (stored-ore) 1) (has-smelter))
			:effect(and(increase(iron) 1) (decrease (stored-ore) 1) (decrease (stored-coal) 1))
	)
	
	(:action produceTool
			:parameters(?blacksmith - person)
			:precondition(and (is-blacksmith ?blacksmith) (has-blacksmith))
			:effect(increase(rifles) 1)
	)
	
	;; --------- Build Building ---------- ;;
	
	(:action buildTurfHut
			:parameters()
			:precondition(and (not(has-turfhut)))
			:effect(and(has-turfhut))
	)
	
	(:action buildHouse
			:parameters(?carpenter - person )
			:precondition(and (is-carpenter ?carpenter)  (not(has-house))
						  (>= (wood) 1))
			:effect(and(has-house) (decrease (wood) 1))
	)
	
	(:action buildSmelter
			:parameters()
			:precondition(and (not(has-smelter)) 
						  (>= (stone) 1))
			:effect(and(has-smelter)  (decrease (stone) 1))
	)
	
	(:action buildSchool
			:parameters()
			:precondition(and (not(has-school))
						  (>= (iron) 1)
						  (>= (wood) 1)
						  (>= (stone) 1))						  
			:effect(and(has-school) (decrease (iron) 1) (decrease (wood) 1) (decrease (stone) 1))
	)
	
	(:action buildSawMill
			:parameters()
			:precondition(and (not(has-sawmill)) 
						  (>= (stone) 1) 
						  (>= (timber) 1) 
						  (>= (iron) 1))
			:effect(and(has-sawmill) (decrease (stone) 1) (decrease (iron) 1) (decrease (timber) 1))
	)
	
	(:action buildBlacksmith
			:parameters()
			:precondition(and (not(has-blacksmith))
						  (>= (stone) 1) 
						  (>= (timber) 1) 
						  (>= (iron) 1))
			:effect(and(has-blacksmith) (decrease (stone) 1) (decrease (iron) 1) (decrease (timber) 1))
	)
	
	(:action buildOreMine
			:parameters(?carpenter - person ?blacksmith - person ?person - person ?resource - resource)
			:precondition(and(is-carpenter ?carpenter) (is-blacksmith ?blacksmith) (ore_resource ?resource) (not(= ?carpenter ?blacksmith)) (not(has-oremine))
						  (>= (wood) 1)
						  (>= (iron) 1))
			:effect(and(has-oremine)  (decrease (wood) 1) (decrease (iron) 1))
	)
	
	(:action buildCoalMine
			:parameters(?carpenter - person ?blacksmith - person ?person - person ?resource - resource)
			:precondition(and(is-carpenter ?carpenter) (is-blacksmith ?blacksmith) (coal_resource ?resource) (not (= ?carpenter ?blacksmith)) (not(has-coalmine))
						  (>= (wood) 1)
						  (>= (iron) 1))
			:effect(and(has-coalmine) (decrease (wood) 1) (decrease (iron) 1))
	)		
	
	(:action buildBarracks
			:parameters(?carpenter - person )
			:precondition(and(is-carpenter ?carpenter) (not(has-barracks))
						  (>= (stone) 1)
						  (>= (wood) 1))
			:effect(and (has-barracks) (decrease (stone) 1) (decrease (wood) 1))
	)
	
	(:action buildQuarry
			:parameters()
			:precondition(and (not(has-quarry)))
			:effect(and (has-quarry))
	)
	
)	
