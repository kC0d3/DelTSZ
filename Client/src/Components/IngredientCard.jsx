import { toast } from 'react-toastify';

export default function IngredientCard({ ingredient, ingredientTypes, fetchProducerIngredients }) {

    const ingredientType = ingredientTypes.find(type => type.index === ingredient.type);

    const handleReceive = async e => {
        e.preventDefault();

        try {
            const recRes = await fetch(`/api/ingredients/${e.target.parentNode.parentNode.id}`, {
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
                fetchProducerIngredients();
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
        <div className='ingredient-card' id={ingredient.id}>
            <div className='ingredient-card-top'>
                <div className='ingredient-card-left'>
                    <img src={`/images/products/${ingredientType.value}.png`} alt={`${ingredientType.value}.png`} />
                </div>
                <div className='ingredient-card-right'>
                    <h2>{ingredientType.value}</h2>
                    <h3>{ingredient.received.slice(0, 10)}</h3>
                    <h3>{ingredient.amount} kg</h3>
                </div>
            </div>
            <div className='ingredient-card-bottom'>
                <button className='ingredient-receive-button' onClick={handleReceive}>Receive</button>
            </div>
        </div>
    );
}