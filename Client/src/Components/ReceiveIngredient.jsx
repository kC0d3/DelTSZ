import { useEffect, useState } from "react";
import IngredientCard from "./IngredientCard";
import LoginError from "./LoginError";

export default function ReceiveIngredient({ ingredientTypes, loggedUser, setShowLogin }) {

    const [producerIngredients, setProducerIngredients] = useState(undefined);

    useEffect(() => {
        fetchProducerIngredients();
    }, []);

    const fetchProducerIngredients = async () => {
        try {
            const ingRes = await fetch('/api/ingredients/producers');
            const ingData = await ingRes.json();

            setProducerIngredients(ingData);
        } catch (error) {
            console.log(error);
        }
    }

    return (
        loggedUser ?
            <div className='receive-ingredient'>
                {
                    producerIngredients &&
                        producerIngredients.length > 1 ? producerIngredients.map((ingredient, index) => (
                            <IngredientCard key={index} {...{ ingredient, ingredientTypes, fetchProducerIngredients }} />
                        ))
                        :
                        <div className='receive-ingredient-none'>
                            <h1>There are no more ingredients to receive at this moment.</h1>
                        </div>
                }
            </div>
            :
            <LoginError {...{ setShowLogin }} />
    );

}