import { useEffect } from "react";
import LoginError from "./LoginError";
import ProfileData from "./ProfileData";
import ProfileProductIngredientList from "./ProfileProductIngredientList";

export default function Profile({ loggedUser, productTypes, ingredientTypes, setShowLogin, fetchUser }) {

    useEffect(() => {
        fetchUser();
    }, []);

    return (
        loggedUser ?
            <div className='profile'>
                <ProfileData {...{ loggedUser }} />
                <ProfileProductIngredientList {...{ loggedUser, productTypes, ingredientTypes }} />
            </div>
            :
            <LoginError {...{ setShowLogin }} />
    );
}