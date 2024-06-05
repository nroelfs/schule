import UserDatatable from '../Components/UserDatatable';
import { Button, Card } from 'antd';
import { UserAddOutlined } from '@ant-design/icons';
import { useState } from 'react';
import CreateOrUpdateUser from '../Components/createOrUpdateUser';

function Register() {
    const [NewModalOpen, setNewModalOpen] = useState(false);

    return (
        <Card
            title="Benutzer Verwaltung"
            extra={
                <Button
                    icon={<UserAddOutlined />}
                    type="text"
                    onClick={() => setNewModalOpen(true)}>
                    Hinzufügen
                </Button>
            }
        >
            <UserDatatable />
            <CreateOrUpdateUser
                user={null}
                isModalOpen={NewModalOpen}
                setIsModalOpen={setNewModalOpen}
            />
        </Card>
    );
}

export default Register;