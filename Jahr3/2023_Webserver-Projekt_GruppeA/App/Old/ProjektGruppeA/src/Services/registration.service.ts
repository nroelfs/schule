import ApiService from "./api.service";

type registerUser = {
    Password: string;
    Username: string;
    Email: string;
    RoleName: string;
    FirstName: string;
    LastName: string;
};
type user = {
    uid:string
    userName: string;
    email: string;
    role: string;
    firstName: string;
    lastName: string;
}
class RegistrationService {

    
    private service = new ApiService('Account');

    async registerNewUser(user: registerUser){
        const token: string = localStorage.getItem('accesstoken') ?? ""
        this.service.setHeader(token);
        console.log(user);
        await this.service.post('/register', {
            'Username': user.Username, 
            'Password': user.Password,
            'Email': user.Email,
            'RoleName': user.RoleName,
            'FirstName': user.FirstName,
            'LastName': user.LastName
        });
    }
    async getAllUser(){
        const token: string = localStorage.getItem('accesstoken') ?? ""
        this.service.setHeader(token);
        const allUser: user[] =  await this.service.get("/getAll");
        return allUser;
    }
    async getUserById(id: string){
        const token: string = localStorage.getItem('accesstoken') ?? ""
        this.service.setHeader(token);
        const user: user = await this.service.get("/get/" + id);
        return user;
    }
    async updateUser(id:string, user:user){
        const token: string = localStorage.getItem('accesstoken') ?? ""
        this.service.setHeader(token);
        await this.service.post("/update/" + id, user);
    }
}
export {RegistrationService};

export default new RegistrationService;