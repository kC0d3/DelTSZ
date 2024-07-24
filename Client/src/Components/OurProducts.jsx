import OurProductCard from "./OurProductCard";

export default function OurProductsBody({ productAndIngredientTypes, productDescriptions }) {
    return (
        <div className='our-products-body'>
            {productAndIngredientTypes.map((product, index) => (
                <OurProductCard key={index} {...{ product }} description={productDescriptions[product.value]} />
            ))}
        </div>
    );
}