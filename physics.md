[TOC]

# 物体属性

基本属性：位置、质心、质量、转动惯量、角度

运动：力、加速度

转动：力矩、角加速度

# 运动学

## 匀速运动

$$
v=\frac{s}{t}
\\
s=vt
$$

## 非匀速运动

### 匀加速度直线运动

$$
v_1=v_0+at
\\
s=v_0t+\frac{1}{2}at^2
$$

### 匀加速度曲线运动

分为x轴和y轴两个方向匀加速度直线运动
$$
v_0=v_{x_0}+v_{y_0}
\\
v_1=v_{x_1}+v_{y_1}
\\
v_{x_1}=v_{x_0}+a_{x}t
\\
v_{y_1}=v_{y_0}+a_{y}t
$$

### 变加速度运动

分为连续的匀加速度运动
$$
v_1=v_0+\int adt
\\
s=\int vdt
$$

## 旋转

刚体绕转动轴旋转

### 恒定角加速度

角位移$\Omega$(rad) 角速度$\omega$(rad/s) 角加速度$a_\omega$
$$
w_2=w_1+a_\omega t
\\
\Omega=\omega_1t+\frac12a_\omega t^2
$$

弧长$c$ 
$$
c=r\Omega
$$
切向线速度$v$
$$
\frac{dc}{dt}=r\frac{d\Omega}{dt}
\\
v=r\omega
$$
切向加速度
$$
\frac{dv}{dt}=r\frac{d\omega}{dt}
\\
a_v=ra_\omega
$$

向心加速度
$$
a_n=\frac{v^2}{r}
\\
a_n=r\omega^2
$$



# 力学

## 重力

$$
G=mg
$$

## 摩擦力

$$
f_{max}=\mu_s m
\\
f_k=\mu_km
$$

## 力与力矩

力是产生直线加速的原因，力矩是产生旋转加速的原因

力的作用线到转动轴的垂直距离$r$
$$
M=rF
$$


# 动理学

## 运动方程

力$F$ 质量$m$ 线加速度$a$

$$
F=ma
$$

力矩$M$ 转动惯量$I$ 角加速度$a$
$$
M=I\alpha
$$

## 一般过程

1. 计算物体的质量属性(质量、质心、惯性矩)
2. 识别加在物体上的力
3. 获得力与力矩的向量
4. 对于线性加速度和角加速度分别解方程
5. 考虑时间因素，求解出线性速度和角速度
6. 考虑时间因素，求解线性位移和角速度



# 碰撞

## 动量与冲量

注意：速度为矢量

### 动量

$$
p=mv
$$

### 冲量

$$
I=F\Delta t=\int Fdt
\\
I=p_1-p0=m(v_1-v_0)
$$

## 碰撞情景

### 动量守恒

$$
m_1v_1+m_2v_2=m_1v_3+m_2v_4
$$

#### 补偿系数

$$
e=-\frac{v_3-v_4}{v_1-v_2}
$$

- 完全非弹性碰撞：$e$为0
- 完全弹性碰撞：$e$为1

### 情景

作用线：垂直于碰撞表明

对心碰撞：速度沿着作用线

倾斜碰撞：速度不是沿着作用线



## 线性冲量和角冲量

### 线性冲量

$J$为冲量
$$
|J|= m_1(|v_{1+}|-|v_{1-}|)
\\
-|J|= m_2(|v_{2+}|-|v_{2-}|)
\\
e=-\frac{|v_{1+}|-|v_{2+}|}{|v_{1-}|-|v_{2-}|}
$$
得出
$$
|v_r|=|v_{1-}-v_{2-}|
\\
|J|=\frac{-|v_r|(e+1)}{\frac{1}{m_1}+\frac{1}{m_2}}
\\
v_{1+}=v_{1-}+\frac{(|J|n)}{m_1}
\\
v_{2+}=v_{2-}+\frac{(-|J|n)}{m_2}
$$

### 角冲量

考虑到线性速度和角速度，则碰撞点的速度为
$$
v_p=v_g+(w\times r)
$$
$r$是从物体重心到点$P$的向量

重写初速度和冲量的方程
$$
v_{1g+}+(w_{1+}\times r_1)=\frac{J}{m_1}+v_{1g-}+(w_{1-}\times r_1)
\\
v_{2g+}+(w_{2+}\times r_2)=\frac{J}{m_2}+v_{2g-}+(w_{2-}\times r_2)
$$
角冲量，$I$为转动惯量，$r$为物体重心到施力点的距离，$r\times J$为矩
$$
(r_1\times J)=I_1(w_{1+}-w_{1-})
\\
(r_2\times J)=I_2(w_{2+}-w_{2-})
$$
得到
$$
|J|=\frac{-(v_r\cdot n)(e+1)}{\frac{1}{m_1}+\frac{1}{m_2}+n\cdot(\frac{r_1\times n}{I_1})\times r_1+n\cdot(\frac{r_2\times n}{I_2})\times r_2}
\\
v_{1+}=v_{1-}+\frac{(|J|n)}{m_1},n为作用线(法线)方向
\\
v_{2+}=v_{2-}+\frac{(-|J|n)}{m_2}
\\
w_{1+}=w_{1-}+\frac{(r_1\times |J|n)}{I_1},n为切线方向
\\
w_{2+}=w_{2-}+\frac{(r_2\times |J|n)}{I_2}
$$

## 碰撞摩擦力

摩擦系数
$$
\mu_k=\frac{F_f}{F_n}
$$
$F_f$是切向方向的摩擦力，$F_n$是法向的碰撞力







