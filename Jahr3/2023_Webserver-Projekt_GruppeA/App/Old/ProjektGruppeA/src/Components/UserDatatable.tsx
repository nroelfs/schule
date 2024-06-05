import RegistrationService from '../Services/registration.service';
import { useEffect, useState } from 'react';
import { Table, Button } from 'antd';
import { EditOutlined } from '@ant-design/icons';
import CreateOrUpdateUser from './createOrUpdateUser';

type User = {
    uid:string
    userName: string;
    email: string;
    role: string;
    firstName: string;
    lastName: string;
};

function UserDatatable() {
    const [users, setUsers] = useState<User[]>([]);
    const [selectedUser, setSelectedUser] = useState<User | null>(null);
    const [editModalOpen, setEditModalOpen] = useState(false);

    useEffect(() => {
        async function fetchUsers() {
            const response = await RegistrationService.getAllUser();
            console.log(response);
            setUsers(response);
        }
        fetchUsers();
    }, []);

    const openEdit = (record: User) => {
        setSelectedUser(record);
        //console.log(record);
        setEditModalOpen(true);

    };

    const columns = [
        {
            title: 'Benutzername',
            dataIndex: 'userName',
        },
        {
            title: 'Vorname',
            dataIndex: 'firstName',
        },
        {
            title: 'Nachname',
            dataIndex: 'lastName',
        },
        {
            title: 'E-mail',
            dataIndex: 'email',
        },
        {
            title: 'Berechtigungsstufe',
            dataIndex: 'role',
        },
        {
            title: '',
            key: 'action',
            render: (record: User) => (
                <Button icon={<EditOutlined />} type="text" onClick={() => openEdit(record)} />
            ),
        },
    ];

    const tableColumns = columns.map((item) => ({ ...item }));

    return (
        <>
            <Table
                columns={tableColumns}
                dataSource={users}
                onRow={(record) => {
                    return {
                        onClick: () => {
                            setSelectedUser(record);
                        },
                    };
                }}
            />
            <CreateOrUpdateUser
                user={selectedUser}
                isModalOpen={editModalOpen}
                setIsModalOpen={setEditModalOpen}
            />
        </>
    );
}

export default UserDatatable;
