import { useNavigate } from 'react-router-dom';
import AuthService from '../Services/auth.service.ts';
import { Button, Form, Input, Card} from 'antd';
import { LockOutlined, UserOutlined } from '@ant-design/icons';
type userCreds = {
    username: string,
    password: string
}

async function login(creds: userCreds){     
    await AuthService.authorize(creds.username, creds.password);
}


function onFinishFailed(){
    console.log('Failed:');
}

function Login(){
    const navigate = useNavigate();
    const onSubmit = async (creds: userCreds) => {
        try {
            await login(creds);
            const isAuthorized = AuthService.isAuthorized();
            if (isAuthorized) {
                navigate("/");
            }
        } catch (error) {
            console.error("Fehler beim Login:", error);
        }
    }
    

    return (
        <>
            <Card title="Login" style={{ maxWidth: 600, left: '40%', right: '50%', marginTop: '10%'}}>
                <Form
                style={{textAlign: 'center'}}
                labelCol={{ span: 8 }}
                wrapperCol={{ span: 16 }}
                autoComplete='on'
                onFinish={onSubmit}
                onFinishFailed={onFinishFailed}>
                    <Form.Item<userCreds>
                        label="Benutzername" 
                        name="username" 
                        rules={[{required: true, message: 'Bitte geben Sie einen Benutzernamen an!'}]}>
                        <Input 
                            prefix={<UserOutlined className="site-form-item-icon" />} 
                            placeholder="Benutzername"/>
                    </Form.Item>
                    <Form.Item<userCreds>
                        label="Passwort" 
                        name="password" 
                        rules={[{required: true, message: 'Bitte geben Sie ein Passwort an!'}]}>
                        <Input  
                            prefix={<LockOutlined className="site-form-item-icon" />}
                            type="password"
                            placeholder="Passwort"/>
                    </Form.Item>
                    <Form.Item wrapperCol={{offset: 8, span: 16}}>
                        <Button type='primary' htmlType='submit'>
                            Anmelden
                        </Button>
                    </Form.Item>
                </Form>
            </Card>
        </>
    )
}
export default Login