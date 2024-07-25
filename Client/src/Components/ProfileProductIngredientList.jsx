import ProfileIngredientCard from "./ProfileIngredientCard";
import ProfileProductCard from "./ProfileProductCard";

export default function ProfileProductIngredientList({ loggedUser, productTypes, ingredientTypes }) {

    return (
        loggedUser.ingredients.length <= 0 && loggedUser.products.length <= 0 ?
            <div className='profile-product-ingredient-list-none'>
                <h1>User do not have any products.</h1>
            </div>
            :
            <div className='profile-product-ingredient-list'>
                {
                    loggedUser.products.length <= 0 ? <></> :
                        <div className='profile-product-list'>
                            <h1>Products</h1>
                            <div className='profile-products'>
                                {
                                    loggedUser.products.map((product, index) => (
                                        <ProfileProductCard key={index} {...{ product, productTypes }} />
                                    ))
                                }
                            </div>
                        </div>
                }
                {
                    loggedUser.ingredients.length <= 0 ? <></> :
                        <div className='profile-ingredient-list'>
                            <h1>Ingredients</h1>
                            <div className='profile-ingredients'>
                                {
                                    loggedUser.ingredients.map((ingredient, index) => (
                                        <ProfileIngredientCard key={index} {...{ ingredient, ingredientTypes }} />
                                    ))
                                }
                            </div>
                        </div>
                }
            </div>
    );
}