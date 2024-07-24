import { useState } from "react";
import { toast } from 'react-toastify';
import LoginError from "./LoginError";

export default function ProvideIngredient({ ingredientTypes, loggedUser, setShowLogin }) {

    const [ingredient, setIngredient] = useState({ type: '', amount: '' });

    const handleProvide = async e => {
        e.preventDefault();

        try {
            const provRes = await fetch('/api/ingredients', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    type: Number(ingredient.type),
                    amount: Number(ingredient.amount)
                })
            });

            const provData = await provRes.json();

            if (provRes.ok) {
                toast.success(provData.message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
                setTimeout(() => {
                    setIngredient({ type: '', amount: '' })
                }, 1000);
            } else {
                toast.error(provData.message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
            }
        } catch (error) {
            console.log(error);
        }
    }

    return (loggedUser ?
        <form className='provide-ingredient' onSubmit={handleProvide}>
            <h1>Provide Ingredient</h1>
            <select className='ingredients' value={ingredient.type} onChange={e => setIngredient({ ...ingredient, type: e.target.value })}>
                <option value='' disabled hidden>Select ingredient</option>
                {ingredientTypes.map((ingredient, index) => (<option key={index} value={ingredient.index}>{ingredient.value}</option>))}
            </select>
            <div className='input-container'>
                <input type='number' step='0.001' aria-label='Quantity' value={ingredient.amount} onChange={e => setIngredient({ ...ingredient, amount: e.target.value })} />
                <label className='unit-label'>kg</label>
            </div>
            <button type='submit' className='provide-button'>Provide</button>
        </form> : <LoginError {...{ setShowLogin }} />
    );
}