import Footer from '../Components/Footer';
import Header from '../Components/Header';
import Body from '../Components/OurProductsBody';

export default function OurProductsPage({ loggedUser, setLoggedUser, bgImagesAmount, productTypes, productDescriptions }) {
    return (
        <div className="our-products-page">
            <Header {...{ loggedUser, setLoggedUser, bgImagesAmount }} />
            <Body {...{ productTypes, productDescriptions }} />
            <Footer />
        </div>
    );
}