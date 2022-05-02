import math
import numpy as np
import matplotlib.pyplot as plt


def f(x):
    return x**0.5


def process(i, x):
    n = len(x)
    r = [1.0]
    for j in range(n):
        if j != i:
            r.append(x[j])
            r[0] /= x[i] - x[j]
    return r


def lagrange(x):
    n = len(x)
    y = [f(val) for val in x]
    L = []
    for i in range(n):
        li = process(i, x) ## считаются многочлены влияния
        li[0] *= y[i] ## fi * li
        L.append(li) ## добавляются слагаемые в итоговый многочлен лангранжа
    return L


def newton(x):
    n = len(x)
    y = [[f(val) for val in x]] ## подставляем значение x в формулу
    k = 1
    N = [[y[0][0]]] ## массив массивов
    for i in range(n - 1):  ## заполняем таблицу конечных разностей
        y.append([])
        
        for j in range(len(y[i]) - 1): ## заполняем таблицу конечных разностей
            y[i + 1].append((y[i][j] - y[i][j + 1]) /
                            (x[j] - x[j + k]))
        N.append([y[i+1][0]] + [x[k] for k in range(i + 1)]) ## нам нужны [y+1][0], будет содержать все слагаемые многочлена
        k += 1
    return N


def printer(P):
    operator = False
    for each in P:
        if operator:
            if each[0] < 0:
                print(f'- {abs(each[0]):5.3f}', end=' ')
            else:
                print(f'+ {abs(each[0]):5.3f}', end=' ')
            for i in range(1, len(each)):
                if each[i] > 0:
                    print(f'(x - {each[i]:3.1f})', end=' ')
                else:
                    print(f'(x + {abs(each[i]):3.1f})', end=' ')
        else:
            if each[0] < 0:
                print(f'- {abs(each[0]):5.3f}', end=' ')
            else:
                print(f'{abs(each[0]):5.3f}', end=' ')
            operator = True
    print('\n', end='')


def mm(lst):
    r = 1
    for i in lst:
        r *= i
    return r


def analyse(x, P):
    func = 0
    for each in P:
        func += each[0] * mm(x - each[i] for i in range(1, len(each))) ## each, умножаем на произведение скобок(для всех иксов берем игрик координату)
    return func


def show_plot(limit, x, y, step=0.1):
    X = np.arange(x[0], x[-1], step)
    Y = []
    for i in range(len(limit)):
        Y.append([limit[i](val) for val in X])

    fif, ax = plt.subplots()

    for i in range(len(Y)):
        ax.plot(X, Y[i], label=f'Function {i}')

    ax.plot(x, y, label='Standard')
    ax.legend(loc='upper right')

    ax.grid()

    plt.show()


if __name__ == '__main__':

    ## приблизить многочленами лангранжа и ньютона, что такое экстрополяция

    x_1 = [0, 1.7, 4.0, 5.1]
    x_check = 0.8

    print('Lagrange')
    L = lagrange(x_1)
    print("L(x) =", end=' ')
    printer(L)

    Lx = analyse(x_check, L)
    print(f'L({x_check}) = {Lx:6.3f}\n')

    print('Newton')
    N = newton(x_1)
    print("N(x) =", end=' ')
    printer(N)

    Nx = analyse(x_check, N)
    print(f'N({x_check}) = {Nx:6.3f}\n')

    print('Function')
    Fx = f(x_check)
    print(f'F({x_check}) = {Fx:6.3f}\n')
    print('Error')
    print(f'|F(x) - L(x)| = {abs(Fx - Lx)}')
    print(f'|F(x) - N(x)| = {abs(Fx - Nx)}')

    def l(x): return analyse(x, L)
    def n(x): return analyse(x, N)
    show_plot([l, n], x_1, [f(val) for val in x_1]) ## подставляем разные иксы из дано, и получаем разные игрики
