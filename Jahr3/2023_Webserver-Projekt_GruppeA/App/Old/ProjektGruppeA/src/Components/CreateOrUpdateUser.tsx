import { Form, Input, Button, Modal } from 'antd';
import RegistrationService from '../Services/registration.service';
import { useEffect } from 'react';

type RegisterUser = {
    userName: string;
    password: string;
    email: string;
    role: string;
    firstName: string;
    lastName: string;
};
type User = {
    uid: string;
    userName: string;
    email: string;
    role: string;
    firstName: string;
    lastName: string;
    };
interface CreateOrUpdateUserProps {
    user: User | null;
    isModalOpen: boolean;
    setIsModalOpen: (open: boolean) => void;
}
function CreateOrUpdateUser({
    user,
    isModalOpen,
    setIsModalOpen,
}: CreateOrUpdateUserProps) {
    const [form] = Form.useForm();

    const onFinishNewUser = (values: RegisterUser) => {
        RegistrationService.registerNewUser(values).then(() => {
            setIsModalOpen(false);
        });
    };

    const onFinishUpdateUser = (values: User) => {
        if(user){
            values.uid = user.uid;
            values.role = user.role;
            RegistrationService.updateUser(user.uid, values).then(()=>{
                setIsModalOpen(false);
            });
        }

    };

    const onCancelModal = () => {
        setIsModalOpen(false);
    };

    useEffect(() => {
        if (user) {
            form.setFieldsValue({
                Username: user.userName,
                Email: user.email,
                RoleName: user.role,
                FirstName: user.firstName,
                LastName: user.lastName,
            });
        } else {
            form.resetFields();
        }
    }, [user, form]);

    return (
        <Modal
            title={user ? `Bearbeite Benutzer: ${user.firstName}` : 'Neuen Benutzer Erstellen'}
            open={isModalOpen}
            onCancel={onCancelModal}
            footer={null}
            width={1000}
            style={{textAlign:"center"}}
        >
            <Form
                form={form}
                labelCol={{ span: 4 }}
                wrapperCol={{ span: 14 }}
                layout="horizontal"
                onFinish={user ? onFinishUpdateUser : onFinishNewUser}
                style={{ maxWidth: 1000 }}
            >
            <Form.Item
            label="Benutzername"
            name="Username"
            rules={[{ required: true, message: 'Bitte Benutzernamen eingeben' }]}
            >
            <Input />
            </Form.Item>
            {!user && ( 
                <>
                    <Form.Item
                    label="Passwort"
                    name="Password"
                    rules={[
                        {required: true, message: 'Bitte Passwort eingeben',},
                    ]}>
                        <Input.Password />
                    </Form.Item>
                    <Form.Item
                        label="Passwort Bestätigen"
                        name="passwordConfirm"
                        dependencies={['Password']}
                        rules={[{required:true}, ({getFieldValue}) => ({
                            validator(_, value) {
                                if (!value || getFieldValue('Password') === value) {
                                    return Promise.resolve();
                                }
                                return Promise.reject(new Error('Die Passwörter müssen gleich sein!'));
                            },
                        }),
                    ]}>
                        <Input.Password/>
                    </Form.Item>
                </>
            )}
            <Form.Item
            label="Email"
            name="Email"
            rules={[{ required: true, message: 'Bitte Email eingeben' }]}
            >
            <Input />
            </Form.Item>
            <Form.Item
            label="Berechtigung"
            name="RoleName"
            rules={[{ required: true, message: 'Bitte Berechtigung eingeben' }]}
            >
            <Input />
            </Form.Item>
            <Form.Item
            label="Vorname"
            name="FirstName"
            rules={[{ required: true, message: 'Bitte Vorname eingeben' }]}
            >
            <Input />
            </Form.Item>
            <Form.Item
            label="Nachname"
            name="LastName"
            rules={[{ required: true, message: 'Bitte Nachname eingeben' }]}
            >
            <Input />
            </Form.Item>
            <Form.Item wrapperCol={{ offset: 4, span: 14 }}>
            <Button type="primary" htmlType="submit">
                Submit
            </Button>
            </Form.Item>
        </Form>
    </Modal>);
}

export default CreateOrUpdateUser;
