import { DropItem } from "./dropItem";

export interface Location {
        locationId: number;
        naam: string;
        adres: string;
        postalCode: string;
        country: string;
        image: string;
        refHospitals: string;
        standardRef: string;
        email: string;
        contact: string;
        contact_image: string;
        telephone: string;
        fax: string;
        logo: string;
        mrnSample: string;
        sMS_mobile_number: string;
        sMS_send_time: string;
        triggerOneMonth: string;
        triggerTwoMonth: string;
        triggerThreeMonth: string;
        dBBackend: string;
        vendors: DropItem[];

}

