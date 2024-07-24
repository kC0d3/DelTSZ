import { useEffect, useState } from "react";
import LoginError from "./LoginError";
import ProductCard from "./ProductCard";
import IngredientCard from "./IngredientCard";

export default function Products({ productTypes, ingredientTypes, loggedUser, setShowLogin }) {

    const [ownerProducts, setOwnerProducts] = useState(undefined);
    const [ownerIngredients, setOwnerIngredients] = useState(undefined);

    useEffect(() => {
        fetchOwnerProductsAndIngredients();
    }, []);

    const fetchOwnerProductsAndIngredients = async () => {
        try {
            await fetchOwnerProducts();
            await fetchOwnerIngredients();
        } catch (error) {
            console.log(error);
        }
    }

    const fetchOwnerProducts = async () => {
        try {
            const prodRes = await fetch('/api/products/owner');
            const prodData = await prodRes.json();

            setOwnerProducts(prodData);
        } catch (error) {
            console.log(error);
        }
    }

    const fetchOwnerIngredients = async () => {
        try {
            const ingRes = await fetch('/api/ingredients/owner');
            const ingData = await ingRes.json();

            setOwnerIngredients(ingData);
        } catch (error) {
            console.log(error);
        }
    }

    return (
        loggedUser ?
            <div className='product-list'>
                {
                    ownerProducts &&
                        ownerIngredients &&
                        ownerProducts.length <= 0 && ownerIngredients.length <= 0 ?
                        <div className='product-list-none'>
                            <h1>There are no more products to order at this moment.</h1>
                        </div>
                        :
                        <>
                            {
                                ownerProducts &&
                                ownerProducts.length > 0 &&
                                ownerProducts.map((product, index) => (
                                    <ProductCard key={index} {...{ product, productTypes, fetchOwnerProductsAndIngredients }} />
                                ))
                            }
                            {
                                ownerIngredients &&
                                ownerIngredients.length > 0 &&
                                ownerIngredients.map((ingredient, index) => (
                                    <IngredientCard key={index} {...{ ingredient, ingredientTypes, fetchOwnerProductsAndIngredients }} />
                                ))
                            }
                        </>
                }
            </div>
            :
            <LoginError {...{ setShowLogin }} />
    );
}