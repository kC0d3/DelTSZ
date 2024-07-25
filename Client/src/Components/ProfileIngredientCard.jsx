export default function ProfileIngredientCard({ ingredient, ingredientTypes }) {
    const ingredientType = ingredientTypes.find(type => type.index === ingredient.type);

    return (
        <div className='profile-ingredient-card' id={ingredient.id}>
            <div className='profile-ingredient-card-top'>
                <div className='profile-ingredient-card-left'>
                    <img src={`/images/products/${ingredientType.value}.png`} alt={`${ingredientType.value}.png`} />
                </div>
                <div className='profile-ingredient-card-right'>
                    <div className='profile-ingredient-card-rigth-top'>
                        <h2>{ingredientType.value}</h2>
                        <h3><span>Amount:</span></h3>
                        <h3>{ingredient.amount} kg</h3>
                    </div>
                    <div className='profile-ingredient-card-rigth-bottom'>
                        <h3><span>Received:</span></h3>
                        <h3>{ingredient.received.slice(0, 10)}</h3>
                    </div>
                </div>
            </div>
        </div>
    );
}