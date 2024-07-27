import { useState } from 'react';
import { toast } from 'react-toastify';

export default function IngredientCard({ ingredient, ingredientTypes, fetchOwnerProductsAndIngredients }) {
    const [amount, setAmount] = useState('');
    const ingredientType = ingredientTypes.find(type => type.index === ingredient.type);

    const handleOrder = async e => {
        e.preventDefault();

        try {
            const recRes = await fetch(`/api/ingredients/${e.target.id}/${Number(amount.replace(',', '.'))}`, {
                method: 'PUT'
            });
            const recData = await recRes.json();

            if (recRes.ok) {
                toast.success(recData.message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
                setAmount('');
                fetchOwnerProductsAndIngredients();
            } else {
                toast.error(recData.message, {
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

    return (
        <form className='ingredient-card' id={ingredient.type} onSubmit={handleOrder}>
            <div className='ingredient-card-top'>
                <div className='ingredient-card-left'>
                    <img src={`/images/products/${ingredientType.value}.png`} alt={`${ingredientType.value}.png`} />
                </div>
                <div className='ingredient-card-right'>
                    <h2>{ingredientType.value}</h2>
                    <h3>Avaiable: {ingredient.amount} kg</h3>
                    <div className='ingredient-card-input-container'>
                        <input type='number' step='0.01' aria-label='Quantity' placeholder='0.00' value={amount} onChange={e => setAmount(e.target.value)} />
                        <label className='ingredient-card-unit-label'>kg</label>
                    </div>
                </div>
            </div>
            <div className='ingredient-card-bottom'>
                <button type='submit' className='ingredient-order-button'>Order</button>
            </div>
        </form>
    );
}