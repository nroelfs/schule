import { Col, Row, Avatar, Badge, Space, Dropdown } from 'antd';
import {UserOutlined, BellOutlined, LogoutOutlined} from '@ant-design/icons';
import type { MenuProps } from 'antd';
import AuthService from '../Services/auth.service';
import { useNavigate } from 'react-router-dom';
function ProfileSlim(){
  const navigate = useNavigate()
  const logout = (()=>{
    AuthService.logout();
    if(!AuthService.isAuthorized()) navigate('/login')
  })
  const items: MenuProps['items'] = [
    {key: '1', label: 'Profil Anzeigen'},
    {key: '2', label:'abmelden',danger: true, icon:<LogoutOutlined /> , onClick: (()=> logout())}
  ]
  return AuthService.isAuthorized() ? (
      <>
          <Row>
          <Col span={21}></Col>
          <Col span={3}>
            <Space size={24}>
              <Badge count={99}>
                <BellOutlined style={{ fontSize: '20px'}}/>
              </Badge>
              <Dropdown menu={{items}} placement="bottomRight" arrow >
                <Avatar  icon={<UserOutlined />} />
              </Dropdown>
            </Space>
          </Col>
          </Row>
      </>
  ) : (
      <>
      <Row>
        <Col span={20}></Col>
        <Col span={4} style={{textAlign : 'right', marginLeft: '-20px'}}>
          Nicht Angemeldet
        </Col>
      </Row>
      </>
  );   
}
export default ProfileSlim